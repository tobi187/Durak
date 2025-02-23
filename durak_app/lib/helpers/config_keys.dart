import 'package:global_configuration/global_configuration.dart';

class ConfigKeys {
  static const String _backendUrl = "backend_url";
  static const String _supabaseUrl = "supabase_url";
  static const String _supabasePubKey = "supabase_pub_key";

  static String get backendUrl => GlobalConfiguration().getValue(_backendUrl);
  static String get supabaseUrl => GlobalConfiguration().getValue(_supabaseUrl);
  static String get supabaseKey =>
      GlobalConfiguration().getValue(_supabasePubKey);
}
