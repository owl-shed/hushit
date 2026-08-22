# Data

The data project will be used in the [engine](../../engine/Engine/) and
[interaction](../../interaction/Interaction/) projects. It will contain the
data models, repositories, and whatever is needed to serialise/store this data.


## Architecture

The current architecture approach to the data models follows the repository
pattern, which provides basic `CRUD` operations, along with more specialised
ones for each repository.

Every type of data get's its own repository, i.e.
[`ITrackInfo`](./Tracks/ITrackInfo.cs) has
[`ITrackRepository`](./Tracks/ITrackRepository.cs).

The repository approach is useful as it allows us to essentially de-duplicate
memory, by using a shared singleton for each data model, i.e. only one instance
of a specific track will ever exist in memory.

The data models themselves also provide helper functions like `Update` and
`Remove`, but these will always redirect to the functions on the repository.

### Updating

For now, the update approach that I went with, is for the data model/repository
to provide a callback which can be used to build an update, for example:

```cs
await trackRepository.UpdateAsync(someTrack, update =>
  update
    .WithName("newTrackName")
    .AddArtist(someArtist)
);

// Or (with the helper method)
await someTrack.UpdateAsync(update =>
  update
    .WithName("newTrackName")
    .AddArtist(someArtist)
);
```

The repository will then take care of serialising this new data, and updating
the original model to match the new state.

## Track metadata

The current plan for extracting track metadata from audio files, is to have a
layer of separation. Which will be to:

1. Load an [`IAudioFileInfo`](./AudioFiles/IAudioFileInfo.cs), which will
  contain the raw metadata fields.
2. Use the raw metadata to find/create artists, albums, genres, tracks, etc.

This separation will let us:

- Have much faster loading times, as we'll have an internal database *(just a
  bunch of json files - nothing too fancy)* that we'll be able to load in a much
  easier to understand format, instead of having to load in the audio file
  metadata each time - that will only be done if the file changes.

- Separate metadata from what the audio file can store, meaning Hushit is
  non-destructive to the audio files, and won't ever have to modify them
  *(or do some weird hybrid).* It also means that we don't actually rely on
  the file metadata, and everything can instead be added by the user if they
  wish to do so.

- Have additional metadata. Since we have complete freedom over our internal
  representation, this means we can allow for however much additional metadata
  we would like, in whatever format we need it to be in.

- Deduplicate metadata. As an example, typically each audio file will store
  the same album cover image, even if every song will end up having the same
  image *(although sometimes songs in an album have unique covers!)*. We'll
  instead be able to check if those images are the same, and then deduplicate
  them.
  And since we can deduplicate them, it also means it's much safer to do things
  like having several resolutions of the image, to optimise for different
  purposes.
