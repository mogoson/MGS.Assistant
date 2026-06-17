[TOC]

# MGS.Inspector

## Summary

- Expansion for Unity Inspector tab.

## Ability

- Invoke method of MonoBehaviour script by custom button from Inspector tab.

## Usage

1. Add the InspectorButtonAttribute to method in your MonoBehaviour script.
  ```text
  //[InspectorButton("Button Display Name")]
  [InspectorButton]
  void Start()
  ```
2. Add your script to GameObject and the Button for method will display in Unity Inspector tab.
3. Click the button and the method will be invoked.

---

Copyright © 2026 Mogoson.	mogoson@outlook.com