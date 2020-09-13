# HowLongItTakesToGetTo

The `HowLongDoesItTakesToGetTo` project is an SPA built with Angular that connects to the API for using the specified functionality.

## Dependencies

This project needs the `RailwayService` API running in the background. The app expects that the API is running under `https://localhost:44325/`.
If you run the API on docker or another port was used please change the config in `src\assets\api.config.ts`.

## Running

```
npm install
npm run start
```

## Possible Improvements

Due to time constraints there are some "Missing" features that I would normally implement in a "Real World" Project. Some of them are the following:

- Separate home page from app component and use Angular Routing properly.
- Separate select and button into specific components under a shared module with proper unit tests.
- Setup different running commands for running API locally or on Docker (currently the app is statically configured to search for the API in port 44325).
- Add unit tests for journeys.service.ts and the home page component.
- Instead of hardcoding the stations the app, a new endpoint could be added to the API so we can retrieve all stations on startup.
