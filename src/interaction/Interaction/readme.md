# Interaction

The interaction project serves as a separation layer between the
[engine](../../engine/Engine/) project, and the [client](../../clients/)
projects.

It will use the mediator pattern to to allow the clients to simply say what they
want to do, or what data they want to get, without having to care about what
services are needed for it.

After the [engine](../../engine/Engine/) project, this will be the secondary
place where most of the Hushit functionality is located.
