// GENERATED CODE - DO NOT MODIFY BY HAND

part of '../bearer.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Bearer _$BearerFromJson(Map<String, dynamic> json) => Bearer(
  accessToken: json['accessToken'] as String,
  expiresIn: (json['expiresIn'] as num).toInt(),
  refreshToken: json['refreshToken'] as String,
);

Map<String, dynamic> _$BearerToJson(Bearer instance) => <String, dynamic>{
  'accessToken': instance.accessToken,
  'expiresIn': instance.expiresIn,
  'refreshToken': instance.refreshToken,
};
