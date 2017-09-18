﻿Homework Write Up

For my homework assignment, I built a tool for a task I actually need to do- manage and compare lists
of contacts across several services. Although this tool probably aught to be a desktop app, I did it
as a WebAPI powered SPA, as that seems to be the thrust of the job as it has been explained to me.
I have gone a bit overboard on structure, my intent being to show how I would build such as app
if it was intended to be a lot bigger than it is, and to demonstrate and explain some of my own
best coding practices. Please forgive my use of heavy duty software engineering stuff here- IOC, 
full unit tests, complex directory layouts, etc. If I was tasked with making this app for a customer,
or, say, a small in-house tool, I would likely use a considerably simpler tech stack to make development 
quick. I firmly believe in being practical with my code, simple solutions for simple problems. 

I started with the built-in MVC Single Page Application template. I then proceeded to reorganize a 
lot of it and remove a lot of stuff that wasn’t mine or that I did not intend to use. My desire is 
to showcase my code and not Microsoft’s. Given the purpose of my app, all the authentication stuff 
was cruft and unnecessary.

I started on the client side. The basic template includes KnockoutJS, which is my favorite MVVM 
framework (and the one I know best), and I immediately brought in require.js as a module loader. 
I have a lot of experience with require.js and have never really felt the need for things like Webpack 
and Browserify; the features they bring have been unnecessary for even very large scale application 
development. My first task was building main.ts, my loader script. It includes an extremely basic 
framework for organizing Knockout components, bundled together with configuration code for require.js.

On the C# side, I started with creating the ContactController as a WebAPI controller. I first brought
in Ninject as my kernel for IOC, which I don't really need