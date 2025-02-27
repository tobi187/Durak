import 'package:durak_app/views/game_screen.dart';
import 'package:durak_app/views/home_screen.dart';
import 'package:durak_app/views/login_screen.dart';
import 'package:go_router/go_router.dart';
import 'package:supabase_flutter/supabase_flutter.dart';

GoRouter createRoutes() {
  final supabase = Supabase.instance.client;
  final isLoggedIn = supabase.auth.currentUser != null;

  return GoRouter(
    initialLocation: isLoggedIn ? '/' : '/login',
    routes: [
      GoRoute(path: '/', builder: (context, state) => HomeScreen()),
      GoRoute(path: '/game', builder: (context, state) => GameScreen()),
      GoRoute(path: '/login', builder: (context, state) => LoginScreen()),
    ],
  );
}
