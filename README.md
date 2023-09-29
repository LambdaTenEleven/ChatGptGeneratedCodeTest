# Practical tasks

## First task

### Description:

Create a RESTful API to manage a simple todo list application using ASP.NET Core, EF and MySQL. The application should allow users to create, read, update, and delete todo items. Each item should have a title and a description. Use EF.Core to persist the items in the database.

- Was it easy to complete the task using AI?

There were some difficulties, but it was easy to complete the task using Chat GPT 4. The generated app is a simple monolith that uses DB context directly in controllers. It is not a good practice, but it is enough to complete the task.

- How long did task take you to complete? (Please be honest, we need it to gather anonymized statistics)

It took me about 1 hour to complete the task.

- Was the code ready to run after generation? What did you have to change to make it usable?

The code was almost ready to run after generation. I had to move the generated code for Startup.cs to Program.cs to make it usable. Also there was a problem with builder.Configuration in Program.cs.

- Which challenges did you face during completion of the task?

I had to move the generated code for Startup.cs to Program.cs to make it usable. Also there was a problem with builder.Configuration in Program.cs.

- Which specific prompts you learned as a good practice to complete the task?

Nothing special.

## Second task

### Description:

Implement a RESTful API for a simple online bookstore using ASP.NET Core and EF. The API should allow users to perform CRUD operations on books, authors, and genres. Books should have a title, author, genre, price, and quantity available. Users should be able to search for books by title, author, or genre. Use EF to persist data to a relational database.

- Was it easy to complete the task using AI?

It was slightly worse because ChatGPT 4 gave broken test code. I had to fix it manually and consult it why the test were failing.

- How long did task take you to complete? (Please be honest, we need it to gather anonymized statistics)

About 2 hours.

- Was the code ready to run after generation? What did you have to change to make it usable?

I had to fix generated test code.

- Which challenges did you face during completion of the task?

It kept giving me broken test code.

- Which specific prompts you learned as a good practice to complete the task?

Nothing special.
