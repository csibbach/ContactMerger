Homework Write Up

For my homework assignment, I built a tool for a task I actually need to do- manage and compare lists
of contacts across several services. Although this tool probably aught to be a desktop app, I did it
as a WebAPI powered SPA, as that seems to be the thrust of the job as it has been explained to me.
I have gone a bit overboard on structure, my intent being to show how I would build such as app
if it was intended to be a lot bigger than it is, and to demonstrate and explain some of my own
best coding practices. Please forgive my use of heavy duty software engineering stuff here- IOC, 
full unit tests, complex directory layouts, etc. If I was tasked with making this app for a customer,
or, say, a small in-house tool, I would likely use a considerably simpler tech stack to make development 
quick. I firmly believe in being practical with my code, simple solutions for simple problems. Along 
these lines, I decided to make this only work for one user- despite being a web app- as anything else
would require persistent storage between requests and one more layer of complexity.

I started with the built-in MVC Single Page Application template. I then proceeded to reorganize a 
lot of it and remove a lot of stuff that wasn’t mine or that I did not intend to use. My desire is 
to showcase my code and not Microsoft’s. Given the purpose of my app, I used the default authentication
system. There was also a lot of horribly out of date stuff in there still- noteably,
jQuery 1.10 and Knockout 2.3... very old. I copied a lot of libraries (qunit, knockout, etc) from
another project I had laying around, I like the layout for that better than the .nuget versions.
This does mean I may not have absolutely the latest versions of the libraries but they are newer
than what was there.

I started on the client side. The basic template includes KnockoutJS, which is my favorite MVVM 
framework (and the one I know best), and I immediately brought in require.js as a module loader. 
I have a lot of experience with require.js and have never really felt the need for things like Webpack 
and Browserify; the features they bring have been unnecessary for even very large scale application 
development. My first task was building main.ts, my loader script. It includes an extremely basic 
framework for organizing Knockout components. I also brought in Infuse.js as my IOC container for the 
client side, and wrapped it inside kernel.ts. I was going to skip this but I'm so used to having 
injection available for view models that I was having a hard time working without it. I created a 
view model factory that supports injection for knockout and configured that for use, and copied in 
a few of my favorite utilities for working with Promises. Overall, the framework is lightweight and 
pretty powerful but doesn't have all the bells and whistles that Angular brings by default.

On the C# side, I started with creating the ContactController as a WebAPI controller. I first brought
in Ninject as my kernel for IOC as I am most familiar with it, but I've used others. The authentication 
stuff took a long time to figure out. I haven't worked on the API side of things for a few years,
and figuring out how to make OAuth work securely in the way I want it to was very difficult. I ended
up having to remove the authentication flow from the SPA itself- I had a great plan to do this
with iframes but Google does not allow that. I had to make the process of adding a contact account
redirect the whole page and come back to the SPA. To make things easier, I used a singleton structure 
in GoogleCredentialProvider. This system works as is, but would become ungainly in production, not to 
mention losing everything on a reastart. My first improvement would be a much better persistent data 
store, but this was getting complicated enough as is. Fortunately, it is abstracted in a way that 
makes this upgrade pretty trivial.

Security was always on my mind as I built out the back end. I tried to follow the basic best practices
for security- making sure things are authenticated up front at least- but generally each layer should
have it's own authentication. For instance, things like ContactProvider would authenticate requests
for data by itself; right now it returns whatever data it has for that username. The username is 
coming from the authentication layer so I think it's OK, but I would have liked to go crazy on this
and it just wasn't worth it. I also started by making API-only controllers ApiControllers, but in 
order to authenticate to them, I need to either bubble up the security token used by the main
login, or re-auth on the client side and get a new token, both of which would be significant work.
In the end I opted to just use pass-through auth, but my endpoints are thus only useable within
my own app. I also opted to skip a purely RESTful API design, instead opting for endpoints that
do things my app needs. This gave me a reason to put more work on the C# side rather than the 
frontend, hopefully it's enough.

Once I had everything bootstrapped, it was a simple matter of TDD based addition. Add in API calls 
in C#, build a front end component in Typescript, write tests, rinse repeat etc. I saved the styling 
for the very last. One, because I suck at design. I can make CSS to match a comp but ask me to work on 
my own design, fat chance. Two, because the nature of building front ends this way allows me to seperate 
the style from the function very well. The app can be completely working and tested without any CSS 
required.

Running The App
You need to put the provided secrets.json file at the root of the file structure, and then hit play in
Visual Studio. IIS Express is configured with SSL and a fixed port, all the google credentials are set
up for it to run in this way. I did not deploy the app anywhere. You'll need to create an account
and login with that to see the main app.

Running Tests
The C# unit tests are all in the ContactMerger.Tests project, and run in the normal way. Typescript
tests are all in the main project, in the Scripts/tests folder. Right click on them and choose Open
In Browser. The PhantomJS engine that Chutzpah uses is missing a lot of features that I need to provide
polyfills for but haven't taken the time.