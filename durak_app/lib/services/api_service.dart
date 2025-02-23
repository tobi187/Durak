import 'dart:convert';

import 'package:durak_app/helpers/config_keys.dart';
import 'package:durak_app/models/api/room.dart';
import 'package:durak_app/models/api/user.dart';
import 'package:http/http.dart' as http;
import 'package:logging/logging.dart';

class ApiService {
  final String backendUrl = ConfigKeys.backendUrl;

  final logger = Logger('ApiService');

  Future<User?> createUser(String? user) async {
    final url = Uri.https(backendUrl, '/api/user/create', {'username': user});
    final resp = await http.get(url);

    if (resp.statusCode != 200) {
      logger.warning('Failed creating user. Res: ${resp.body}');
      return null;
    }
    final json = jsonDecode(resp.body) as Map<String, dynamic>;
    return User.fromJson(json);
  }

  Future<Iterable<Room>> getOpenRooms() async {
    final url = Uri.https(backendUrl, '/api/room/getAll');
    final resp = await http.get(url);

    if (resp.statusCode != 200) {
      logger.warning('Failed getting RoomList. Res ${resp.body}');
      return [];
    }
    final json = jsonDecode(resp.body) as List<Map<String, dynamic>>;
    return json.map(Room.fromJson);
  }

  Future<String?> createRoom(String? name, User user) async {
    final url = Uri.https(backendUrl, '/api/room/create', {'roomName': name});
    final resp = await http.post(url, body: user.toJson());

    if (resp.statusCode != 200) {
      logger.warning("Failed creating Room. Res ${resp.body}");
      return null;
    }
    return resp.body;
  }

  Future<String?> joinRoom(String id, User user) async {
    final url = Uri.https(backendUrl, '/api/room/join', {'roomId': id});
    final resp = await http.post(url, body: user.toJson());

    if (resp.statusCode != 200) {
      logger.warning("Failed creating Room. Res ${resp.body}");
      return null;
    }
    return resp.body;
  }
}
