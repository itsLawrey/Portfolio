# Inter process communication project

A small C project in which we have a parent process, which is able to manipulate a bin file containing poem entries.

### Functions

- Add new poem
- Delete poem
- Edit poem
- List poems
- Send child to tell a poem

There are 4 children, and a random one gets chosen when we send out a child to tell a poem. The child recieves 2 poems through a pipe from the parent, then chooses one to delete. It sends the other back to the parent via message queue.

This project helped me really understand signals in C, and low-level file operations.
