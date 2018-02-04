# The Cool Tapes Galaxy
A Spehssmen Simulator for Advanced Neckbeards

# What?
You heard me. <a href="https://github.com/SkulkingScavenger/the-cool-tapes-galaxy/wiki">Go to The Wiki</a> if you want the fluff.

# How you can Help 

Use that wiki to contribute concepts/lore/etc./etc. This is a **Collaborative Effort** so make up whatever you want and put it in. We'll make tonal adjustments later.

# How you can Help WITH THE GAME

You need to pull down the code, add your changes, and make a pull request.

For loading assets, you just need **git**, a **github account** and **editing rights**. If you have those, proceed. If not, you can...<br> 
 * download git for free <a href="https://git-scm.com/downloads">here</a><br>
 * create a free account <a href="https://github.com/join?source=header-home">here</a><br>
 * send me your correctly spelled **github username** at SkulkingScavenger@gmail.com or on discord<br>
 
 If you want to test your changes (good idea), or play the game, you need Unity3D which you can get for free <a href="https://unity3d.com/">here</a>
 
 # Getting and Uploading the Project Using Git (guide for non-programmers)

NOTE: The following instructions use terminal commands. If you're using a GUI client, use its method instead of the terminal command for each step.

<b>If you have not already cloned the repository:</b><br>
1. Open up terminal (or git bash if you chose to install it) and navigate to the directory you want to keep the project in.<br>
2. clone the repository<br>
`git clone https://github.com/SkulkingScavenger/the-cool-tapes-galaxy.git`<br>
3. checkout a new branch<br>
`git checkout -b the-name-of-your-new-branch`<br>
4. Make your changes, probably you want to add pictures in the `Assets/Sprites` folder, but maybe you also want to change some data in the `Assets/Scripts` folder
5. Finally, push your changes to the cloud<br>
`git add .` (don't forget the period and exact spacing. its git shorthand for 'all')<br>
`git commit -m "a short description of what you changed"`<br>
`git push`<br>
Probably a message will come up and tell you to run a different version of push, just do whatever it says

<b>If you already have the repository cloned:</b><br>
1. pull down the latest version to avoid conflicts<br>
`git pull`<br>
2. checkout a new branch with the `-b` flag or checkout an existing one<br>
old: `git checkout some-existing-branch`<br>
new: `git checkout -b my-new-branch`<br>
3. make changes
4. push your changes<br>
`git add .`<br>
`git commit -m "your commit message here"`<br>
`git push`<br>
