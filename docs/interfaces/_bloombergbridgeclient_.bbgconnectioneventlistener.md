[finsemble-bloomberg](../README.md) › [Globals](../globals.md) › ["BloombergBridgeClient"](../modules/_bloombergbridgeclient_.md) › [BBGConnectionEventListener](_bloombergbridgeclient_.bbgconnectioneventlistener.md)

# Interface: BBGConnectionEventListener

Interface representing an event handler for connection events, which are fired
when the BloombergBridge connects or disconnects from the terminal.

## Hierarchy

* function

  ↳ **BBGConnectionEventListener**

## Callable

▸ (`err`: string | Error, `response`: RouterMessage‹object›): *void*

*Defined in [src/clients/BloombergBridgeClient/BloombergBridgeClient.ts:19](https://github.com/ChartIQ/finsemble-bloomberg/blob/fd42a96/src/clients/BloombergBridgeClient/BloombergBridgeClient.ts#L19)*

Interface representing an event handler for connection events, which are fired
when the BloombergBridge connects or disconnects from the terminal.

**Parameters:**

Name | Type |
------ | ------ |
`err` | string &#124; Error |
`response` | RouterMessage‹object› |

**Returns:** *void*

▸ (`err`: E, `response?`: R): *void*

Defined in node_modules/@finsemble/finsemble-core/types/types.d.ts:12

Interface representing an event handler for connection events, which are fired
when the BloombergBridge connects or disconnects from the terminal.

**Parameters:**

Name | Type |
------ | ------ |
`err` | E |
`response?` | R |

**Returns:** *void*
