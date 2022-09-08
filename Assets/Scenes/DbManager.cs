using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DbManager
{
    public static string username;
    public static string password;
    public static string Role;
    public static int score;

    public static string AdminInformation;

    public static string AdminUsername;

    public static string StudentInformation;

    public static string ClassName;

    public static bool loggedIn { get { return username != null; }}

    public static void logOut()
    {
        username = null;
    }
}
