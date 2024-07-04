# CSharpApp

An application for C# knowledge assessment.

# Description

This is a web application that interacts with a 3nd party service (https://jsonplaceholder.typicode.com) and serves data

The application has some problems described below that need to be addressed plus some new features that need implementation.

## Problems

**#1**

Seems that there is a problem in the current implementation of HttpClient that not handles the requests properly.

**#2**

Τhere is one or more problems/bugs that exist! It would be great if you catch them and attempt to fix.

## New features

**#1**

Create a new service that handles the "Posts" endpoint (/posts)

The following must be supported:

- Get all
- Get by id
- Add new post
- Delete post

**#2**

Create a generic http client wrapper handling all types of requests.

The following verbs must be supported:
- Get
- Post
- Put
- Delete