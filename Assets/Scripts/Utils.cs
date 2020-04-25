using System.Text.RegularExpressions;
using UnityEngine;

public class Utils : MonoBehaviour {

    // This regex expression checks the validity of the email inputted.
    const string regexExpression = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z";

    public static bool IsEmailValid(string email) {
        var regex = new Regex(regexExpression, RegexOptions.IgnoreCase);
        return regex.IsMatch(email);
    }


}
