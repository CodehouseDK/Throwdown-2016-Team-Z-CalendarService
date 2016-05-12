#Codehouse Throwdown 2016 - Team Z
This is a template project for Codehouse Throwdown 2016, use it if you like.

##Getting started
Install ASP.NET Core - [Installing Asp.net 5 On Windows](https://docs.asp.net/en/latest/getting-started/installing-on-windows.html)

Install NodeJs - [Windows Installer](https://nodejs.org/dist/v6.1.0/node-v6.1.0-x64.msi)

Install Docker - [Get Started With Docker For Windows](https://docs.docker.com/windows/) or [Sign up for Docker Beta (Do it anyway and today)](https://beta.docker.com/)

###Dependency Injection
I have configured a simple service, to demo the dependency injection of Asp.Net Core.
read more here - [Dependency Injection](https://docs.asp.net/en/latest/fundamentals/dependency-injection.html)


##Local dev
Restore packages
 > \\> dnu restore
 
Run project
 > \\> dnx -p project.json web
 
###Frontend
I have create this template with WebPack
 > \\> npm install
  
####build once
 > \\> webpack
 
####Watch for changes
 > \\> webpack --watch
 
Disclaimer:
This is my favorit build tool for frontend, you dont need to use it. It is advanced and can do alot more than i have configured if for. 
Please use you favorit build tool, i have configured the package.json so you can easily change to what ever you like.
to use grunt change
```
 "scripts": {   
 "build": "webpack"
 },
```
to

```
 "scripts": {   
 "build": "grunt"
 },
```

##Docker

Build the docker container 
 > \\> docker build -t codehouse:teamz .

Run the container on port 8080
 > \\> docker run --name teamz -p 8080:5004 codehouse:teamz
 
Stop a running container, you need to do this when you have a new build
 > \\> docker stop teamz
 > \\> docker rm teamz 
 
###Docker Compose
I have create an example file with a redis server, you can use this client
  `"StackExchange.Redis": "1.1.572-alpha"` 

[StackExchange.Redis - Basic usage](https://github.com/StackExchange/StackExchange.Redis/blob/master/Docs/Basics.md)

More on Docker-compose - [Getting Started](https://docs.docker.com/compose/gettingstarted/)


##Inspiration
 * [Micro.js](http://microjs.com/#) - for all your microframework needs
 * [Bacon.js](https://baconjs.github.io/) - In case you want to play with Reactive
 * [Cognitive Services](https://www.microsoft.com/cognitive-services) - For all your cognitive needs
 * [Dockerize Asp.net](http://kjanshair.azurewebsites.net/Blog/DockerizeAspNetCore)

 #Help
 Go find me, and i will do the best to help you. 
 Please remember this should be fun og you should make something that works **once**, and the most important things to learn are
  * Asp.net Core
  * Docker
If you don't feel at home in vanilla js or fancy build tool use what you like.