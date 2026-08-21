# Audio sub-projects

The [main audio project](Audio/Audio.csproj) *(`./Audio/`)* will contain the
necessary base types/interfaces for audio playback. Will additional sub-projects
for each audio backend that we want to experiment with.

This will allow us to separate the native dependencies for each backend we try,
so that the [client projects](../clients/) don't get bloated.
