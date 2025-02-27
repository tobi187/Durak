import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:supabase_flutter/supabase_flutter.dart';

class LoginScreen extends StatelessWidget {
  LoginScreen({super.key});
  final supabase = Supabase.instance.client;
  final GlobalKey<FormState> _formKey = GlobalKey<FormState>();
  final mailText = TextEditingController();
  final pwText = TextEditingController();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Padding(
        padding: EdgeInsets.all(40),
        child: Column(
          spacing: 20,
          children: [
            Text(
              "World of Durak",
              style: Theme.of(context).textTheme.headlineLarge,
            ),
            Form(
              key: _formKey,
              child: Column(
                spacing: 20,
                children: [
                  TextFormField(
                    controller: mailText,
                    decoration: InputDecoration(hintText: "Email"),
                    validator: (value) {
                      final mailRegex = RegExp("");
                      if (value == null || value.isEmpty) {
                        return "Email eingeben";
                      }
                      if (!mailRegex.hasMatch(value)) {
                        return "Email nicht valid";
                      }
                      return null;
                    },
                  ),
                  TextFormField(
                    controller: pwText,
                    decoration: InputDecoration(hintText: "Password"),
                    obscureText: true,
                    validator: (value) {
                      if (value == null || value.length < 6) {
                        return "Passwortlänge > 5";
                      }
                      return null;
                    },
                  ),
                  FilledButton(
                    onPressed: () async {
                      if (!_formKey.currentState!.validate()) {
                        return;
                      }
                      final res = await supabase.auth.signInWithPassword(
                        email: mailText.text,
                        password: pwText.text,
                      );
                      if (res.user == null) {
                        return;
                      }
                      if (context.mounted) {
                        context.go('/');
                      }
                    },
                    child: Text("Log in"),
                  ),
                  FilledButton(
                    onPressed: () async {
                      final res = await supabase.auth.signInAnonymously();
                      if (res.user == null) {
                        return;
                      }
                      if (context.mounted) {
                        context.go('/');
                      }
                    },
                    child: Text("Als Gast spielen"),
                  ),
                  FilledButton(
                    onPressed: () async {
                      if (!_formKey.currentState!.validate()) {
                        return;
                      }
                      final res = await supabase.auth.signUp(
                        email: mailText.text,
                        password: pwText.text,
                      );
                      if (res.user == null) {
                        return;
                      }
                      if (context.mounted) {
                        context.go('/');
                      }
                    },
                    child: Text("Registrieren"),
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
}
