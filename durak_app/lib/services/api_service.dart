import 'dart:convert';

import 'package:durak_app/helpers/config_keys.dart';
import 'package:durak_app/models/api/bearer.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:logging/logging.dart';
import 'package:http/http.dart' as http;

class ApiResult {
  final int statusCode;
  final Map<String, dynamic> body;
  final bool success;
  bool get authFail => statusCode == 401 || statusCode == 403;
  bool get hasValue => statusCode < 300 && body.isNotEmpty;

  ApiResult({
    required this.statusCode,
    required this.success,
    required this.body,
  });

  factory ApiResult.fromHttpResult(http.Response resp) {
    final success = resp.statusCode < 300;
    Map<String, dynamic> json = {};
    // or try catch maybe
    try {
      json = jsonDecode(resp.body);
    } catch (_) {} // just return empty map on fail

    return ApiResult(statusCode: resp.statusCode, success: success, body: json);
  }

  factory ApiResult.fromException() {
    return ApiResult(statusCode: 500, success: false, body: {});
  }
}

class ApiService {
  final String backendUrl = Config.backendUrl;
  final logger = Logger('ApiService');
  Bearer? _bearer;

  Future<bool> _authenticate() async {
    final url = _getUri('/anon');
    final res = await _call(url, "post");
    if (!res.hasValue) {
      return false;
    }
    _bearer = Bearer.fromJson(res.body);
    return true;
  }

  Future<bool> _authenticateIfNeeded() async {
    if (_bearer != null) {
      if (_bearer!.isValid) {
        return true;
      } else {
        return await _authenticate();
      }
    } else {
      final storage = FlutterSecureStorage();
      final refreshKey = await storage.read(key: "");
    }
    return await _authenticate();
  }

  Future<ApiResult> get(String url, {Map<String, dynamic>? query}) async {
    final authRes = await _authenticateIfNeeded();
    if (!authRes) {
      return ApiResult.fromException();
    }
    final gUrl = _getUri(url, query: query);
    final res = await _call(gUrl, "get");

    return res;
  }

  Future<ApiResult> post(
    String url, {
    Map<String, dynamic>? query,
    Object? body,
  }) async {
    final authRes = await _authenticateIfNeeded();
    if (!authRes) {
      return ApiResult.fromException();
    }
    final gUrl = _getUri(url, query: query);
    final res = await _call(gUrl, "post", body: body);

    return res;
  }

  Future<ApiResult> _call(Uri url, String method, {Object? body}) async {
    try {
      final req = http.Request(method, url);
      if (_bearer?.accessToken != null) {
        req.headers["Authorization"] = "Bearer ${_bearer!.accessToken}";
      }
      if (body != null) {
        var jsonBody = jsonEncode(body);
        req.body = jsonBody;
      }
      var resStream = await req.send();
      var res = await http.Response.fromStream(resStream);

      return ApiResult.fromHttpResult(res);
    } catch (e, s) {
      logger.severe("Api Call to $url failed", e, s);
      return ApiResult.fromException();
    }
  }

  Uri _getUri(String path, {Map<String, dynamic>? query}) {
    if (Config.isProd) {
      return Uri.https(backendUrl, path, query);
    }
    return Uri.http(backendUrl, path, query);
  }
}
