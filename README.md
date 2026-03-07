# Tessera

A desktop drawing application built with Avalonia UI and .NET, using an MVVM architecture with a data-driven canvas rendering approach.

## Overview

Tessera is a vector drawing tool that supports multiple shape types, canvas manipulation, and a modular tool system. Shapes are rendered through an `ItemsControl` hosting a `Canvas` as its `ItemsPanelTemplate`, where each shape is a ViewModel bound to a corresponding DataTemplate.

## Features

- **Drawing tools** — Line, rectangle, ellipse, polyline (free drawing), point, and text
- **Text tool** — Click-and-drag to define a text area, type to edit, finalize on blur or Escape
- **Shape options** — Configurable stroke color, thickness, dash patterns, fill types (none, semi-transparent, solid), and opacity per tool
- **Eraser** — Rectangle-based eraser with line-segment intersection detection
- **Canvas navigation** — Pan (drag) and zoom (scroll wheel, buttons) with anchor-to-cursor behavior
- **Grid overlay** — Configurable grid with dots, lines, and crosses types; adjustable spacing and color; scales correctly with zoom
- **Canvas options** — Dialog for grid spacing, type, and color customization
- **Zoom controls** — Zoom in/out buttons, reset zoom with viewport centering, percentage display

## Tech Stack & Architecture

- **Framework:** Avalonia UI (.NET)
- **Pattern:** MVVM with CommunityToolkit.Mvvm (`ObservableObject`, `RelayCommand`, source generators)
- **Canvas rendering:** Data-driven via `ItemsControl` + `ItemsPanelTemplate` with `Canvas`
- **Transform system:** `CanvasTransform` class using Avalonia's `Matrix` type for unified pan/zoom, with `ToWorld`/`ToScreen` coordinate conversion
- **Tool system:** `ICanvasTool` interface with `OnPointerPressed`, `OnPointerMoved`, `OnPointerReleased`; tools receive an `ICanvasContext` interface rather than the full ViewModel
- **Grid rendering:** Custom `CanvasGrid` control drawing in world coordinates via `RenderTransform`, with automatic spacing adjustment at extreme zoom levels
- **Shape templates:** `ShapeTemplateSelector` maps shape models to their visual DataTemplates

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- An IDE with Avalonia support (Rider, Visual Studio, VS Code with Avalonia extension)

### Build & Run

```bash
git clone <repo-url>
cd Tessera
dotnet restore
dotnet run --project Tessera.App
```

## Roadmap

- **Shape selection** — Click-to-select, marquee select, multi-select with Ctrl+click, drag-to-move selected shapes
- **Undo/Redo** — Command pattern with undo stack for shape operations
- **Scripting console** — Embedded code editor (AvaloniaEdit) with syntax highlighting for programmatic drawing via a scripting API
- **Pointer capture** — Explicit capture handling for reliable drag interactions across window boundaries
