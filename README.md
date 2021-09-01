# graph
playing with graphs. no, not pie charts. go away now.

## why?
every problem that matters can be solved with graphs. for instance, if you know the answer to the ultimate question of life, the universe and everything is 42, then all you have to do is traverse from node 42 to find the question. simple, right?

## semantic networks
[![semantic network image from wikipedia](https://upload.wikimedia.org/wikipedia/commons/6/67/Semantic_Net.svg)]

## references

- https://www.youtube.com/watch?v=aRo6na52tZs
- https://en.wikipedia.org/wiki/Graph_theory
- https://en.wikipedia.org/wiki/Graph_database
- https://graphdatabases.com/
- http://graphml.graphdrawing.org/
- https://graphql.org/
- https://www.youtube.com/watch?v=h9wxtqoa1jY&list=PL6MpDZWD2gTF3mz26HSufmsIO-COKKb5j&index=1

## notes

### variable structure & connections - primitives:
- nodes / vertices
- relationships / edges
- properties / attributes
- labels (labels are types, classifications or stereotypes)

### consider the context of a use case model:
 - the terms "actor" and "use case" would be labels on nodes representing actors and use cases
 - attributes would be used to name the nodes, for example "login" might be the name of a use case and "player" might be the name of an actor in a model describing a game
 - stereotypes &lt;&lt;extends&gt;&gt; and &lt;&lt;includes&gt;&gt; would be labels on edges that define assocaiations between use cases
 - a use case model is a chain graph (contains both directed and undirected edges). associations between actors and use cases are undirected. associations between use cases are directed. inheritance relationships between actors is directed.
 - user roles (for security or other user segregation) can be discovered by creating forests from the traveral of the use cases coupled to each actor

