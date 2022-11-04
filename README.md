# MessagePackTools

Quick and dirty tool to deserialize MessagePack token-by-token for troubleshooting purposes. Example output (slightly truncated):

```
> dotnet run C:\temp\msgpack-tracechunk-good.bin

Loaded 899 bytes from 'C:\temp\msgpack-tracechunk-good.bin'.

Array:1
  [0]:Array:2
    [0]:Map:11
      [0] key:String:trace_id
          value:Integer:1575375132615536630
      [1] key:String:span_id
          value:Integer:7514245303430381383
      [2] key:String:name
          value:String:child
      [3] key:String:resource
          value:String:child
      [4] key:String:service
          value:String:service2
      [5] key:String:type
          value:Nil
      [6] key:String:start
          value:Integer:1667581651878834900
      [7] key:String:duration
          value:Integer:2870100
      [8] key:String:parent_id
          value:Integer:3257881236536141923
      [9] key:String:meta
          value:Map:18
        [0] key:String:0
            value:String:0
        [1] key:String:1
            value:String:1
        [2] key:String:2
            value:String:2
(...)
        [14] key:String:14
             value:String:14
        [15] key:String:runtime-id
             value:String:57df5ba4-053b-4bad-a2cc-13298cfd7516
        [16] key:String:env
             value:String:Overridden Environment
        [17] key:String:language
             value:String:dotnet
      [10] key:String:metrics
           value:Map:17
        [0] key:String:_dd.limit_psr
            value:Float:0.75
        [1] key:String:0
            value:Float:0
        [2] key:String:1
            value:Float:1
(...)
        [15] key:String:14
             value:Float:14
        [16] key:String:_dd.top_level
             value:Float:1
    [1]:Map:10
      [0] key:String:trace_id
          value:Integer:1575375132615536630
      [1] key:String:span_id
          value:Integer:3257881236536141923
      [2] key:String:name
          value:String:root
      [3] key:String:resource
          value:String:root
      [4] key:String:service
          value:String:service1
      [5] key:String:type
          value:Nil
      [6] key:String:start
          value:Integer:1667581651872471200
      [7] key:String:duration
          value:Integer:11011600
      [8] key:String:meta
          value:Map:4
        [0] key:String:_dd.p.dm
            value:String:-0
        [1] key:String:runtime-id
            value:String:57df5ba4-053b-4bad-a2cc-13298cfd7516
        [2] key:String:env
            value:String:Overridden Environment
        [3] key:String:language
            value:String:dotnet
      [9] key:String:metrics
          value:Map:5
        [0] key:String:_dd.tracer_kr
            value:Float:0
        [1] key:String:_dd.agent_psr
            value:Float:1
        [2] key:String:process_id
            value:Float:2236
        [3] key:String:_sampling_priority_v1
            value:Float:1
        [4] key:String:_dd.top_level
            value:Float:1

Done.
```
