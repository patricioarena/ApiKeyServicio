### Introducción
Servicio de autenticación de un cliente para el uso de los servicios o aplicaciones.
######
#### Cosas a tener en cuenta
######
(Llamare APP indistintamente al servicio o aplicacion al que se le quiere dar acceso, atravez del servicio de autenticacion apikey)
######
Todos los las tablas en el esquema ApiKey que contengan un campo enable se cargan por defecto en false a menos que se carge desde el servicio mediante los endpoints establecidos.
######
1. Registrar un cliente desde endpoint **/api/Client/Register** setea enable en true automaticamente.
######
2. Genererar una key mediante el endpoint **/api/Key/AssignKey** para un cliente **NO** la setea en enable se la debe dar de alta mediante el endpoint **/api/Key/Enable**.
######
3. Para dar acceso a una APP mediante una key se debe usar el endpoint **/api/Key/GrantAppAccess**. Si la key y el cliente estan habilitados el acceso a la APP es automatico pero si el cliente o la key no estan habilitados no, es decir el campo enable en la tabla de vinculacion estara en false y no tendra acceso.
######
4. Dado que el esquema esta pensado para que una misma key pueda ingresar a varias APPS.
    - Si se desea revocar el acceso a una APP endpoint **/api/Key/RevokeAppAccess**.
    - Si se desea revocar el acceso a todas las APPS endpoint **/api/Key/Disable/{key}**
    - Si se desea deshabilitar un cliente endpoint **/api/Client/Disable/{id}**


### Ambiente de pruebas
######
Cargarla una APP endpoint **/api/Apps/Register** (Si no hay APPs cargadas o no esta la que desea, etc.).
######
1. Registrar un cliente **/api/Client/Register**.
2. Asignarle una key **/api/Key/AssignKey**.
3. Dar de de alta la key **/api/Key/Enable**.
4. Dar a la key acceso a la APP **/api/Key/GrantAppAccess**
5. Listo. :)

- Para verificar que una key tiene acceso a una APP  endpoint **/api/Authentication/VerificationKey**

### Logs
Durante la llamada a **/api/Authentication/VerificationKey** no se guarda registro de las autenticaciones exitosas pero si de las fallidas.


Libreria para facilitar autenticación mediante el presente servicio.
[Ver documentación](https://github.com/patricioarena/ApiKeyLibreria)

    Happy encoding :)
