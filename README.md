# EDCHost

The official host program for Electronic Design Competition

## Install

Download the latest release from [here](https://github.com/THUASTA/EDCHost/releases) and unzip it to an empty folder.

## Usage

Run `EdcHost.exe` to launch the program.

See API references at [Slave API documentation](https://thuasta.github.io/EDCHost/api/slave/) and [Viewer API documentation](https://thuasta.github.io/EDCHost/api/viewer/).

### Configuration

Create a `.env` file under the workspace with content below:

```sh
# Mines
GAME_DIAMOND_MINES="(0, 0) (4, 4)"
GAME_GOLD_MINES="(1, 3)"
GAME_IRON_MINES=""

# Logging
LOGGING_LEVEL=Debug

# RESTful API server
SERVER_PORT=8080
```

## Contributing

If you have any suggestions or improvements, please submit [an issue](https://github.com/THUASTA/EDCHost/issues/new) or a pull request.

## License

[GPL-3.0-only](LICENSE) © THUASTA
