Simple destruction effect for Unity
==================================

[Demo](https://gfycat.com/CheapWildIcelandichorse)

I had a silly idea of how you could do a destruction effect in Unity, and this
is the result. The idea is to generate a Voronoi diagram for the wall, apply a
few rounds of Lloyd relaxation, and then generate meshes from the cells which
becomes the "shards" of the wall. UV coordinates are also assigned to the front
faces of the shards to make up a continuous texture, but that part could be much
improved. The shards are created in Start() and are all kinematic Rigidbodies.

When you "shoot", a raycast is sent to the backwall which is the origin of the
explosion, and all the shards touching the explosion radius become non-kinematic
and has an explosion force applied to them. 

There are many optimizations one might do for this, but the frame-rate holds up
even on my ancient MacBook Pro. Since this was just a quick project, I didn't
clean up the code too much and haven't made extensive comments, but would be
happy to do so if requested. 

To generate the Voronoi diagram I use the (this library for
c#)[https://github.com/PouletFrit/csDelaunay], as experience have taught me that
reimplementing Fortune's algorithm is no fun :)
