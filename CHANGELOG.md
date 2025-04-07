# Change Log

<a name="1.1.0"></a>
## [1.1.0](https://www.github.com/Rael-G/MiniMapr/releases/tag/v1.1.0) (2025-04-07)

### Features

* **performance:** add compiled setter caching using expression trees ([3735dfa](https://www.github.com/Rael-G/MiniMapr/commit/3735dfadbaec3f02fb7acfac76e0d524f8f7be77))
* **performance:** cache property info to avoid repeated reflection ([0bbf286](https://www.github.com/Rael-G/MiniMapr/commit/0bbf28668240ace2967f50b29e55cce2a3c0341c))
* **performance:** use compiled getters to avoid reflection during mapping ([4b7e7dd](https://www.github.com/Rael-G/MiniMapr/commit/4b7e7dde436a8e9f718aa8c6542ff0dd0a7f9bcb))

### Bug Fixes

* **mapper:** throw when destination property is not writable instead of silently skipping ([c220f4b](https://www.github.com/Rael-G/MiniMapr/commit/c220f4bb35b1ed8d0743f58a4ac9ad1f35879638))
* **mapper:** use source property getter instead of destination property getter ([dcc03f6](https://www.github.com/Rael-G/MiniMapr/commit/dcc03f67e72df830a7f3e2f4eff824ba2fd6f927))
* **performance:** avoid repeated value.GetType calls by passing source type explicitly ([abe0b2e](https://www.github.com/Rael-G/MiniMapr/commit/abe0b2ea55242b80f38c497c4557d550c7481028))

<a name="1.0.3"></a>
## [1.0.3](https://www.github.com/Rael-G/MiniMapr/releases/tag/v1.0.3) (2025-04-06)

### Other

* working ci pipeline ([0c4c905](https://www.github.com/Rael-G/MiniMapr/commit/0c4c905bed0eb8052909fdacdc6fceca2eb3729c))

<a name="1.0.2"></a>
## [1.0.2](https://www.github.com/Rael-G/MiniMapr/releases/tag/v1.0.2) (2025-04-06)

