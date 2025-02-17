// GENERATED CODE - DO NOT MODIFY BY HAND

part of '../room.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Room _$RoomFromJson(Map<String, dynamic> json) => Room(
  id: json['id'] as String,
  name: json['name'] as String,
  users:
      (json['users'] as List<dynamic>)
          .map((e) => User.fromJson(e as Map<String, dynamic>))
          .toList(),
  isPlaying: json['isPlaying'] as bool? ?? false,
);

Map<String, dynamic> _$RoomToJson(Room instance) => <String, dynamic>{
  'id': instance.id,
  'name': instance.name,
  'isPlaying': instance.isPlaying,
  'users': instance.users,
};
