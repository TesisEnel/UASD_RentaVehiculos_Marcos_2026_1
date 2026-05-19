# Sistema de Renta de Vehículos
<img width="2044" height="1103" alt="{3F3D38BC-682E-4299-A987-0ECC5AC45EBE}" src="https://github.com/user-attachments/assets/a4b46a7f-375b-4882-ba6f-f6eb1e3cee2e" />

## 1. Descripción general del proyecto

El Sistema de Renta de Vehículos es una plataforma integral diseñada para gestionar el ciclo de vida completo del alquiler de automóviles, desde la reserva inicial hasta la facturación final. El sistema centraliza la administración de una flota de vehículos distribuidos en múltiples sucursales, permitiendo el control operativo de mantenimientos, clientes, empleados y transacciones financieras.

## 2. Módulos principales del sistema

### A. Gestión de flota y vehículos

Este módulo controla el inventario de activos de la empresa.

- **Ficha técnica:** Almacena marca, modelo, año, placa, kilometraje y especificaciones técnicas (combustible, transmisión, capacidad).
- **Multimedia y características:** Gestión de catálogos visuales con imágenes principales y galerías adicionales, junto con listas de amenidades del vehículo (GPS, WiFi, etc.).
- **Mantenimiento preventivo y correctivo:** Registro de costos, proveedores y programación de próximos servicios basados en el kilometraje para asegurar la seguridad de la flota.

### B. Gestión de clientes y usuarios

Administra los perfiles que interactúan con el sistema.

- **Clientes:** Registro detallado que incluye validación de licencias de conducir (fechas de vencimiento) y segmentación por tipo de cliente.
- **Seguridad:** Sistema de autenticación de usuarios con roles definidos y vinculación a sucursales específicas para el personal administrativo.

### C. Operaciones de renta y reservas

El núcleo transaccional del negocio.

- **Reservas:** Permite apartar vehículos con depósitos previos, definiendo sucursales de recogida y entrega.
- **Contratos de renta:** Gestión de alquileres activos, control de kilometraje inicial/final, estados del vehículo y firma de contratos.
- **Servicios extra:** Posibilidad de añadir conceptos adicionales (seguros, asientos para bebés, conductores adicionales) durante la renta.

### D. Administración y finanzas

- **Facturación:** Generación de documentos comerciales que calculan subtotal, impuestos y montos totales según el método de pago elegido.
- **Configuración de reglas de negocio:** Parametrización de horas de gracia, costos por kilómetro extra, porcentajes de multas, edad mínima para rentar y años de experiencia requeridos con la licencia.

## 3. Entidades principales y relaciones

Basado en el diagrama SQL, el sistema se rige por las siguientes relaciones clave:

- **Sucursales como eje geográfico:** Los vehículos, usuarios (empleados) y las operaciones de renta están vinculados a sucursales específicas.
- **Trazabilidad total:** Cada renta (`Rentas`) está conectada directamente con un cliente, un vehículo, un empleado y puede generar una o más facturas o cargos extra.
- **Integridad de datos:** Se utilizan restricciones de clave foránea con borrados en cascada para elementos dependientes como imágenes y características de vehículos, asegurando la limpieza de la base de datos.

## 4. Flujo operativo típico

1. **Reserva:** El cliente selecciona un vehículo y realiza un depósito.
2. **Apertura de renta:** Al recoger el vehículo, se registra el kilometraje inicial, se verifica la licencia y se firma el contrato.
3. **Uso y mantenimiento:** El sistema monitorea si el vehículo requiere servicio técnico según su uso.
4. **Cierre y facturación:** Al devolver el vehículo (posiblemente en una sucursal distinta), se calculan cargos adicionales por retraso o daños, se emite la factura final y se gestiona la devolución del depósito.
