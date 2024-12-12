# CS1400-finalProject-starlink200

The final outcome I was hoping for was to make a volleyball stat maker that could hold data and make sense of it over a course of multiple events and also be able to compare teams to each other using the data given.

I play volleyball and often find many stat makers a little difficult to use but also I'm intrigued by how they work so I
wanted to make a program that did that.

I managed to get my project to get the raw data, make sense of it, and record it. But I was unable to get it to the point where I could use that recorded information at later times within the program like comparing teams to one another or making overall averages.

A big thing that I learned is making methods that make the code more concise and understandable. When I started the project I made a huge chunk of code that would take in the raw data for hitting, and then another huge chunk of almost identical code for digging and then again for serving. So in the hopes of making a more understandable code I made a method that could be universally used to collect the data and then the methods for taking in digging and hitting and servings stats just called the method that would universally take the data.