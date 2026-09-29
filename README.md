# Proyecto - Chat

Este proyecto es un conjunto de dos programas (cliente y servidor) que conforman un chat. Este chat permite la comunicación entre múltiples clientes a través de mensajes privados y salas, así como reconocer el estado de otros usuarios.
El reporte y toda la información de documentación puede encontrarse en el directorio `Docs`.

## Instrucciones de compilación

Dependiendo de su sistema operativo, siga las instrucciones correspondientes.

### Instalación del SDK de .NET
Instale el SDK de .NET en su versión `10.0.401`:

#### Linux
```bash
wget https://dot.net/v1/dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --version 10.0.401
```
Además, configure las variables de entorno:
```bash
export DOTNET_ROOT=$HOME/.dotnet
export PATH=$PATH:$DOTNET_ROOT:$DOTNET_ROOT/tools
```

#### Windows
```cmd
winget install -e --id Microsoft.DotNet.SDK.10
```

Posteriormente, compruebe que se haya instalado la versión correcta mediante el siguiente comando:
```bash
dotnet --version
```
Debe arrojar el número de la versión mencionada.
Si no funciona, puede probar a instalarlo desde la [página oficial de .NET](https://dotnet.microsoft.com/es-es/download/dotnet/10.0), donde se entra más en detalle para diferentes sistemas operativos.

---

### Compilación
Localice los archivos que terminen con el sufijo `.sln` y ejecute el siguiente comando en su ruta:

```bash
dotnet build <ruta_del_archivo>
```

Las rutas de los archivos de la solución válidas son:
* Para compilar el servidor: `Server\Server.sln`
* Para compilar el cliente: `Client\Client.sln`

> **Nota:** Asegúrese de contar con conexión a internet para compilar el cliente, ya que debe realizarse la restauración automática de las dependencias del framework de Avalonia. En caso de que la compilación del cliente falle, puede consultar más información en la [documentación oficial de Avalonia](https://docs.avaloniaui.net/docs/get-started/install-avalonia).

---

## Instrucciones para ejecutar y finalizar

### Servidor
Para ejecutar el servidor escriba el siguiente comando:

```bash
dotnet run --project Server\ChatServer\ChatServer.csproj <numero_de_puerto>
```

Puede ingresar el número del puerto donde desea conectar al servidor, asegurándose que entra en el rango válido. Si no lo ingresa, el servidor se conecta al número de puerto `1234` por defecto. 

Cuando quiera cerrar el servidor, simplemente ejecute el comando `Ctrl + C`.

### Cliente
Para ejecutar el cliente, asegúrese de contar con conexión a internet para la carga inicial de dependencias de Avalonia y escriba el siguiente comando:

```bash
dotnet run --project Client\Client.csproj
```

Esto abrirá una interfaz gráfica con la aplicación del cliente.
