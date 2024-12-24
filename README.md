# LKQ Web API .NET Core 5.0 Template
 
Template for creating .NET Core 5.0 APIs - below are example notes/details to help your project
Template is designed for VS2019 and VSCode 1.50+ with CSharp plugin
Template has Docker built-in, so you can test your app before you send it to devops inside a container (just select docker from the drop down)

## Deployment Constraints/Considerations

This application requires .Net Core 5 and GDIPlus libraries available in the container

## Databases and Services

This application connects with Acme1 and Acme2 SQL servers

## IDP/Security

This application uses the following connection details for QC
```
ClientId: 7e72955f-85ce-4fa1-a663-5eb871944605
Secret:   rkiOZVCZqKCBVjGTVlom
Api:      rulesrepoapi
Scopes:   rulesreporeadwrite          
```