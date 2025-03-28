import 'package:json_annotation/json_annotation.dart';

part 'generated/bearer.g.dart';

@JsonSerializable()
class Bearer {
  final String accessToken;
  final int expiresIn;
  final String refreshToken;
  final DateTime creationTime = DateTime.now();

  Bearer({
    required this.accessToken,
    required this.expiresIn,
    required this.refreshToken,
  });

  factory Bearer.fromJson(Map<String, dynamic> json) => _$BearerFromJson(json);
  Map<String, dynamic> toJson() => _$BearerToJson(this);

  bool get isValid => creationTime
      .add(
        Duration(seconds: expiresIn - 60),
      ) // renew a minute earlier, to eliminate race conditions ?
      .isAfter(DateTime.now());
}
