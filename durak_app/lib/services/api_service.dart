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
  final storage = FlutterSecureStorage();
  Bearer? _bearer;

  Future<bool> authenticate(String url, {Object? body}) async {
    final gUrl = _getUri(url, query: {'useCookies': false});
    final res = await _call(gUrl, "post", body: body);
    if (!res.hasValue) {
      return false;
    }
    _bearer = Bearer.fromJson(res.body);
    await storage.write(key: 'refresh-key', value: _bearer!.refreshToken);
    return true;
  }

  Future<bool> _refreshBearer({String? key}) async {
    final url = _getUri('/refresh');
    key ??= _bearer?.refreshToken;
    final res = await _call(url, 'post', body: {'refreshKey': key});
    if (res.success) {
      _bearer = Bearer.fromJson(res.body);
      return true;
    }
    return false;
  }

  Future<bool> _authenticateIfNeeded() async {
    if (_bearer != null) {
      if (_bearer!.isValid) {
        return true;
      } else {
        return await _refreshBearer();
      }
    } else {
      final refreshKey = await storage.read(key: "refresh-key");
      if (refreshKey != null) {
        return await _refreshBearer(key: refreshKey);
      }
    }
    return false;
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
      if (res.statusCode >= 300) {
        _logApiFail(res, req.body);
      }

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

  void _logApiFail(http.Response resp, String? body) {
    var log = [
      "Api Call to ${resp.request?.url ?? '?'} failed",
      "StatusCode: ${resp.statusCode}",
      "Reason: ${resp.reasonPhrase}",
    ];

    if (resp.body.isNotEmpty) {
      log.add("Body: ${resp.body}");
    }

    if (body != null && body.isNotEmpty) {
      log.add("Request Body: $body");
    }

    logger.warning(log.join("\n"));
  }
}
