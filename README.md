# MessagePackTools

Quick and dirty tool to deserialize MessagePack token-by-token for troubleshooting purposes.

Example output (truncated):

<details><summary>Valid MessagePack.</summary>

```
> dotnet run C:\temp\msgpack-tracechunk-good.bin
Loaded 899 bytes from 'C:\temp\msgpack-tracechunk-good.bin'.

Array (Length=1)
    [0] Array (Length=2)
        [0] Map (Length=11)
            [0] String:"trace_id"
                Integer:1575375132615536630
            [1] String:"span_id"
                Integer:7514245303430381383
            [2] String:"name"
                String:"child"
            [3] String:"resource"
                String:"child"
            [4] String:"service"
                String:"service2"
            [5] String:"type"
                Nil
            [6] String:"start"
                Integer:1667581651878834900
            [7] String:"duration"
                Integer:2870100
            [8] String:"parent_id"
                Integer:3257881236536141923
            [9] String:"meta"
                Map (Length=18)
                [0] String:"0"
                    String:"0"
                [1] String:"1"
                    String:"1"
                [2] String:"2"
                    String:"2"
(...)
                [14] String:"14"
                     String:"14"
                [15] String:"runtime-id"
                     String:"57df5ba4-053b-4bad-a2cc-13298cfd7516"
                [16] String:"env"
                     String:"Overridden Environment"
                [17] String:"language"
                     String:"dotnet"
            [10] String:"metrics"
                 Map (Length=17)
                [0] String:"_dd.limit_psr"
                    Float:0.75
                [1] String:"0"
                    Float:0
                [2] String:"1"
                    Float:1
                [3] String:"2"
                    Float:2
(...)
                [15] String:"14"
                     Float:14
                [16] String:"_dd.top_level"
                     Float:1
        [1] Map (Length=10)
            [0] String:"trace_id"
                Integer:1575375132615536630
            [1] String:"span_id"
                Integer:3257881236536141923
            [2] String:"name"
                String:"root"
            [3] String:"resource"
                String:"root"
            [4] String:"service"
                String:"service1"
            [5] String:"type"
                Nil
            [6] String:"start"
                Integer:1667581651872471200
            [7] String:"duration"
                Integer:11011600
            [8] String:"meta"
                Map (Length=4)
                [0] String:"_dd.p.dm"
                    String:"-0"
                [1] String:"runtime-id"
                    String:"57df5ba4-053b-4bad-a2cc-13298cfd7516"
                [2] String:"env"
                    String:"Overridden Environment"
                [3] String:"language"
                    String:"dotnet"
            [9] String:"metrics"
                Map (Length=5)
                [0] String:"_dd.tracer_kr"
                    Float:0
                [1] String:"_dd.agent_psr"
                    Float:1
                [2] String:"process_id"
                    Float:2236
                [3] String:"_sampling_priority_v1"
                    Float:1
                [4] String:"_dd.top_level"
                    Float:1

Done.
```

</details>

<details><summary>Invalid MessagePack. The first map expects 10 items, but only has 9.</summary>

```
 > dotnet run C:\temp\msgpack-tracechunk-bad.bin
Loaded 639 bytes from 'C:\temp\msgpack-tracechunk-bad.bin'.

Array (Length=1)
    [0] Array (Length=1)
        [0] Map (Length=10)
            [0] String:"trace_id"
                Integer:8555489236270840812
            [1] String:"span_id"
                Integer:7447053569848949053
            [2] String:"name"
                String:"root"
            [3] String:"resource"
                String:"root"
            [4] String:"service"
                String:"ReSharperTestRunner"
            [5] String:"type"
                Nil
            [6] String:"start"
                Integer:1667567770440501600
            [7] String:"duration"
                Integer:11682700
            [8] String:"meta"
                Map (Length=20)
                [0] String:"0"
                    String:"0"
                [1] String:"1"
                    String:"1"
                [2] String:"2"
                    String:"2"
(...)
                [14] String:"14"
                     String:"14"
                [15] String:"_dd.p.dm"
                     String:"-0"
                [16] String:"runtime-id"
                     String:"77f25c4d-5e22-4b4f-be2f-28b5343f8492"
                [17] String:"env"
                     String:"Overridden Environment"
                [18] String:"language"
                     String:"dotnet"
                [19] String:"metrics"
                     Map (Length=21)
                    [0] String:"_dd.limit_psr"
                        Float:0.75
                    [1] String:"_dd.tracer_kr"
                        Float:0
                    [2] String:"_dd.agent_psr"
                        Float:1
                    [3] String:"0"
                        Float:0
                    [4] String:"1"
                        Float:1
                    [5] String:"2"
                        Float:2
(...)
                    [17] String:"14"
                         Float:14
                    [18] String:"process_id"
                         Float:19044
                    [19] String:"_sampling_priority_v1"
                         Float:1
                    [20] String:"_dd.top_level"
                         Float:1

Reached end of bytes, but expected more data.
```

</details>

<details><summary>Invalid MessagePack. Single string, but the last byte is missing.</summary>

```
> dotnet run C:\temp\msgpack-tracechunk-short.bin
Loaded 13 bytes from 'C:\temp\msgpack-tracechunk-short.bin'.

String
Reached end of bytes, but expected more data.
```

</details>
