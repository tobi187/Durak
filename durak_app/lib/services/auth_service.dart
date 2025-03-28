import 'package:durak_app/models/api/user.dart';

class AuthService {
  static User? _user;

  bool get isAuthenticated => _user != null;
  User? get user => _user;

  void init() {}
}
