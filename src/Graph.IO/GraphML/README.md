# GraphML
read/write to GraphML format

## references

- http://graphml.graphdrawing.org/
- http://graphml.graphdrawing.org/specification.html

## notes

GmlReader reads GraphML from a stream, parses it into a GraphML model containing a set of GmlGraph elements, each of which is transformed into a set of Graph.Elements.Graph elements.
GmlWriter transforms a set of Graph.Elements.Graph elements into a GraphML model and writes the model to stream.

