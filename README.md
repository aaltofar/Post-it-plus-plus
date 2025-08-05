# 🖨️ Post-it++

A Discord-powered Raspberry Pi thermal receipt printer bot that prints your chaotic digital thoughts as physical todo notes. Because some problems deserve to exist in the real world.

## 📦 Project Structure
TodoPrinter.sln
- ├── DiscordBot/ - Listens to Discord slash commands like /print
- ├── PrinterApp/ - Core logic: queue, templates, printer handling
- ├── PrinterApi/ - HTTP API for managing queue, settings, templates
- └── PrinterFrontend/ - Blazor WASM frontend for UI access to the queue

## 💡 Features

- 🔗 Discord slash command `/print` to queue messages remotely
- 🧾 Prints to thermal receipt printer via USB (ESC/POS)
- 📋 Persistent local queue (SQLite)
- 🖥️ Web UI to view, manage, and configure print jobs
- 🧠 Template system for project-specific formatting

## 🚀 Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download)
- Raspberry Pi with USB thermal printer (ESC/POS compatible)
- A Discord bot (see below)
- Optional: Web browser for accessing the UI

---
