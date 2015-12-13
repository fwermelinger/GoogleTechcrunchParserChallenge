# GoogleTechcrunchParserChallenge
This was a test project for a client who wanted to see code quality. The requirements were: search google and parse the results, load Techcrunchs website and download all the articles in 3 parallel processes. 
The whole thing was built in only 3 hours. If you have any suggestions, let me know.

Detailed requirements:
======================

Create a windows console application that does the following:

1) Searches Google with a provided query (the query is provided as the first argument to the program) and prints to the console the title of each result & its url(Please do not use Google API for this, but rather through an HTTP request) 

2) Next the program downloads the home page of TechCrunch (http://techcrunch.com). There are 20 article excerpts on the home page. For every article excerpt print to the console the Title, Url to the full article, Author & Excerpt (the short article summary).

3) Using exactly 3 threads, the program downloads all the articles on the home page. For each page downloaded, globally count the frequency of each word on the page.
Global Frequency means how much the same word repeats itself across all the pages the program scans. The frequency count is case insensitive and words with punctuation symbols can be considered as two different words. 
Examples:
- "Hello" and "hello" is the same word. 
- "Hello," and "hello" are two different words.

4) Print the all words found and its frequency in an ascending order.

Instructions:
•	Keep in mind the principles of SoC (Separation of Concerns) & Code Reuse.
•	Code readability is very important, add comments where necessary & give variables meaningful names.
•	Keep things simple. Don’t overkill this task with too many abstractions, interfaces & inheritance. This is not the goal here. However, create the program just with the necessary layers and helper functions that you think is needed.
•	Make the output of the program to look nice and easy to read.
•	Use C# 4.5 & Linq
•	There are numerous ways to parse html pages. Please include a short explanation why you choose a particular method.
