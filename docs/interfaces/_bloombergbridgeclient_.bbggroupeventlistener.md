[finsemble-bloomberg](../README.md) › [Globals](../globals.md) › ["BloombergBridgeClient"](../modules/_bloombergbridgeclient_.md) › [BBGGroupEventListener](_bloombergbridgeclient_.bbggroupeventlistener.md)

# Interface: BBGGroupEventListener

Interface representing an event handler for Bloomberg group events.

## Hierarchy

* function

  ↳ **BBGGroupEventListener**

## Callable

▸ (`err`: string | Error, `response`: RouterMessage‹object›): *void*

*Defined in [src/clients/BloombergBridgeClient/BloombergBridgeClient.ts:35](https://github.com/ChartIQ/finsemble-bloomberg/blob/310ed1f/src/clients/BloombergBridgeClient/BloombergBridgeClient.ts#L35)*

Interface representing an event handler for Bloomberg group events.

**Parameters:**

Name | Type |
------ | ------ |
`err` | string &#124; Error |
`response` | RouterMessage‹object› |

**Returns:** *void*

▸ (`err`: E, `response?`: R): *void*

Defined in node_modules/@finsemble/finsemble-core/types/types.d.ts:12

Interface representing an event handler for Bloomberg group events.

**Parameters:**

Name | Type |
------ | ------ |
`err` | E |
`response?` | R |

**Returns:** *void*
