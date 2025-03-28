import 'package:durak_app/views/game_screen.dart';
import 'package:durak_app/views/home_screen.dart';
import 'package:durak_app/views/login_screen.dart';
import 'package:go_router/go_router.dart';

GoRouter createRoutes() {
  return GoRouter(
    routes: [
      GoRoute(path: '/', builder: (context, state) => HomeScreen()),
      GoRoute(path: '/game', builder: (context, state) => GameScreen()),
      GoRoute(path: '/login', builder: (context, state) => LoginScreen()),
    ],
  );
}
