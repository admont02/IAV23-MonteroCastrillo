# La CafeterIA

Este proyecto final para la asignatura Inteligencia Artificial para Videojuegos (IAV) ha sido realizado por ADRIÁN MONTERO CASTRILLO, realizado en Unity 3D.

## Propuesta

Mi proyecto presenta un juego ambientado en un emocionante bar virtual en el que tú, como jugador, asumes el papel de camarero y atiendes a una variedad de clientes virtuales. La IA que impulsa a estos clientes está construida utilizando máquinas de estado de Bolt.

Características principales:

- Interfaz de bar virtual: Sumérgete en un entorno virtual realista que simula un bar animado con mesas, barra, bebidas y clientes IA.

- Experiencia de camarero interactivo: Como jugador, tomas el control del camarero y te encargas de atender a los clientes, tomando pedidos, agarrando bebidas y sirviéndolas.

- Clientes IA basados en máquinas de estado: Los clientes del bar están impulsados por IA creada utilizando Bolt, lo que les permite exhibir comportamientos realistas y reacciones a tus acciones como camarero.

- Variabilidad y desafío: Cada cliente tiene diferentes preferencias. Debes adaptarte rápidamente y satisfacer sus necesidades para mantenerlos felices.
 
 
 ## Diseño de la solución
 
 En el proyecto dispongo de las siguientes clases:

- PlayerController: Clase para el manejo del jugador (camarero), se moverá mediante NavMesh con las teclas ‘W’, ‘A’, 'S','D' y podrá interactuar con los clientes/bebidas/mesas, con la tecla ‘E’, cogiendo bebidas, hablando/dando bebidas a los clientes y limpiando las mesas pertinentes. Por otro lado, con la tecla 'F' podrá dar las bebidas a los clientes.

- Cliente: Clase para el manejo de los clientes, booleanos y métodos para el control de la máquina de estados

- ClientesManager: Se encarga de generar los clientes

- Mesa: Clase que tienen las mesas del bar para comprobar si están ocupadas y/o sucias.

- Mesas Manager: Sirve para saber si hay alguna mesa vacía y obtenerla, para poder enviar ahí a un cliente



Los clientes funcionan mediante una máquina de estados, según la cual intercalan ir a la barra a pedir su bebida, esperar su bebida, ir a mesa y consumir su bebida. Aquí adjunto una imagen de la máquina implementada en el proyecto:

![image](https://github.com/admont02/IAV23-MonteroCastrillo/assets/82326212/ba98332e-32c4-49c5-84d9-ebead8e575c4)


Cada x segundos, irá entrando un nuevo cliente en el bar, siempre y cuando no se supere el máximo de clientela permitida (7). Lo primero que hará el cliente, lógicamente, será ir a la barra a pedir su bebida, a menos que haya demasiada gente esperando en la barra, entonces, el cliente se pondrá a bailar, una vez haya sitio para pedir, retomará la acción. 

Una vez en la barra, el cliente se irá impacientando, hasta el punto de que, si está demasiado esperando a ser atendido , se marchará.

Una vez atendido, cada cliente deseará una bebida aleatoria entre cerveza, vino y whiskey. Cuando el camarero interactúe por primera vez con el cliente en la barra, aparecerá durante 3 segundos un icono en su cabeza que nos indicará lo que desea tomar. Será labor del camarero recordar que bebida desea cada cliente para tenerle satisfecho, tendrá que ir a la estantería correspondiente, coger la bebida y dársela al cliente, sin demorarse mucho para que no se moleste. 

Cuando le demos su bebida, el cliente irá a una mesa vacía a consumirla tranquilamente, si no quedan mesas libres, se quedará en la barra tomándola. Si por casualidad el cliente se encuentra una mesa sucia, se enfadara aún más, por lo que también es labor del camarero encargarse de que todo esté limpio para que no haya molestias añadidas.

Una vez termine su tiempo de consumición , el cliente abandonará el local.
 
 ## Pruebas y métricas
https://youtu.be/UFwAk5nB_zE
 ## Referencias

- Unity, Bolt Visual Scripting
https://docs.unity3d.com/bolt/1.4/manual/index.html

- Unity, Navegación y Búsqueda de caminos
https://docs.unity3d.com/es/2021.1/Manual/Navigation.html

- Música 
https://assetstore.unity.com/packages/audio/music/fantasy-tavern-music-free-pack-118847

- Assets del bar
https://assetstore.unity.com/packages/3d/props/furniture/medieval-tavern-pack-112546
