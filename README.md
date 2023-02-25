# RedditApiReader
 Analyze New Posts From Reddit

To use this, you will need to get a Reddit API. You can request one here:

https://old.reddit.com/prefs/apps/

Give it an appropriate name, description, and redirect uri (any valid one will do) and select the "web app" option.

Once it is created you will need to find the Client Id (should be underneath the words "web app") and the Client Secret (labled as "secret")

Once located you will need to add them to the user secrets.

Setup your user secrets like so:
{
  "RedditApiWebApp": {
    "ClientId": "YOUR_CLIENT_ID_HERE",
    "ClientSecret": "YOUR_CLIENT_SECRET_HERE"
  }
}

You will also need to run an Update-Database command from the Package Manager Console
Make sure that the appropriate connection string is in the appsettings.json file.

###TODO
Add a countdown to refresh on the pages
