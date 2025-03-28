import 'package:durak_app/models/api/user.dart';
import 'package:json_annotation/json_annotation.dart';

part 'generated/room.g.dart';

@JsonSerializable()
class Room {
  String id;
  String name;
  bool isPlaying;
  List<User> users;

  Room({
    required this.id,
    required this.name,
    required this.users,
    this.isPlaying = false,
  });

  factory Room.fromJson(Map<String, dynamic> json) => _$RoomFromJson(json);

  // Map<String, dynamic> toJson() => _$RoomToJson(this);
  Map<String, dynamic> toJson() => { "id": "a"  };
}
