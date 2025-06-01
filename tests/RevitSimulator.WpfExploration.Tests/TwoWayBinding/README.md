# Two-Way Binding Exploration

This folder contains implementations and tests demonstrating two-way binding between UI controls and view models. The primary goal is to illustrate the fundamental concept of maintaining complete separation between UI components and their data representation.

## Core Concept

The key principle demonstrated here is that a ViewModel should have no knowledge of UI elements. A ViewModel is not business logic - it's a UI-independent representation of what should be displayed. This separation is critical because:

1. **Separation of Concerns**: ViewModels represent the state and data needed by the UI without depending on UI frameworks or components.

2. **Testability**: With decoupled components, ViewModels can be tested independently without UI dependencies.

3. **Maintainability**: Changes to the UI implementation don't require changes to the ViewModel, and vice versa.

4. **Reusability**: The same ViewModel can be used with different UI technologies or presentations.

## Implementations

This folder currently contains:

1. **Custom Implementation**: A from-scratch implementation showing how two-way binding works under the hood using events and reflection.

2. **WPF Implementation**: Examples using WPF's built-in binding system to achieve the same separation.

3. **Stack Overflow Demo**: Demonstrates potential pitfalls when implementing binding without proper safeguards.

Currently, the implementations focus on event-based approaches to two-way binding. Future additions will explore alternative mechanisms for achieving the same separation of concerns.