using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Commit: Adapter - Implementar Adapter para integrar un sistema de pago heredado

namespace pagos
{
    // ─── 1. SISTEMA HEREDADO (no se puede modificar) ───────────────────────
    class SistemaPagoHeredado
    {
        public void RealizarTransaccion(double monto, string moneda)
        {
            Console.WriteLine("[LEGADO] Transacción realizada: " + monto + " " + moneda);
        }

        public string VerificarEstadoTransaccion(int idTransaccion)
        {
            Console.WriteLine("[LEGADO] Verificando transacción ID: " + idTransaccion);
            return "COMPLETADA";
        }

        public void DevolverPago(int idTransaccion, double monto)
        {
            Console.WriteLine("[LEGADO] Devolución procesada para ID: " + idTransaccion + " por " + monto);
        }
    }

    // ─── 2. INTERFAZ OBJETIVO (lo que espera el código nuevo) ──────────────
    interface IProcesadorPago
    {
        void ProcesarPago(double monto);
        string ConsultarEstado(int idPago);
        void Reembolsar(int idPago, double monto);
    }

    // ─── 3. ADAPTER (traduce la interfaz nueva al sistema heredado) ─────────
    class PagoAdapter : IProcesadorPago
    {
        private SistemaPagoHeredado _sistemaHeredado;

        public PagoAdapter(SistemaPagoHeredado sistemaHeredado)
        {
            _sistemaHeredado = sistemaHeredado;
        }

        public void ProcesarPago(double monto)
        {
            Console.WriteLine("[ADAPTER] Traduciendo ProcesarPago → RealizarTransaccion");
            _sistemaHeredado.RealizarTransaccion(monto, "USD");
        }

        public string ConsultarEstado(int idPago)
        {
            Console.WriteLine("[ADAPTER] Traduciendo ConsultarEstado → VerificarEstadoTransaccion");
            return _sistemaHeredado.VerificarEstadoTransaccion(idPago);
        }

        public void Reembolsar(int idPago, double monto)
        {
            Console.WriteLine("[ADAPTER] Traduciendo Reembolsar → DevolverPago");
            _sistemaHeredado.DevolverPago(idPago, monto);
        }
    }

    // ─── 4. CLIENTE (solo conoce la interfaz nueva) ─────────────────────────
    class TiendaOnline
    {
        private IProcesadorPago _procesador;

        public TiendaOnline(IProcesadorPago procesador)
        {
            _procesador = procesador;
        }

        public void RealizarCompra(double monto)
        {
            Console.WriteLine("\n=== Iniciando compra ===");
            _procesador.ProcesarPago(monto);
            string estado = _procesador.ConsultarEstado(101);
            Console.WriteLine("Estado del pago: " + estado);
        }

        public void HacerDevolucion(int idPago, double monto)
        {
            Console.WriteLine("\n=== Procesando devolución ===");
            _procesador.Reembolsar(idPago, monto);
        }
    }

    // ─── 5. PROGRAM (punto de entrada) ──────────────────────────────────────
    internal class Program
    {
        static void Main(string[] args)
        {
            // Sistema heredado existente
            SistemaPagoHeredado sistemaViejo = new SistemaPagoHeredado();

            // Adapter envuelve el sistema heredado
            IProcesadorPago adapter = new PagoAdapter(sistemaViejo);

            // Cliente usa interfaz moderna sin saber nada del sistema viejo
            TiendaOnline tienda = new TiendaOnline(adapter);

            tienda.RealizarCompra(250.00);
            tienda.HacerDevolucion(101, 250.00);

            Console.ReadKey(); // Pausa para ver la salida
        }
    }
}