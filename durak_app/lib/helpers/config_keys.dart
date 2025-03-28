import 'package:flutter_dotenv/flutter_dotenv.dart';

class Config {
  static const String _backendUrl = "backend_url";

  static String get backendUrl => dotenv.get(_backendUrl);
  static bool get isProd =>
      dotenv.get("environment", fallback: "production") == "production";
}
