"use client"

import { ColumnDef } from "@tanstack/react-table"

export type Bill = {
    id: number
    name: string
    amountDue: number
    amountPaid?: number
    isPaid: boolean
}

export const columns: ColumnDef<Bill>[] = [
    {
        accessorKey: "name",
        header: "Name"
    },
    {
        accessorKey: "amountDue",
        header: () => <div className="text-right">Amount Due</div>,
        cell: ({ row }) => {
            const amountDue = parseFloat(row.getValue("amountDue"))
            const formatted = new Intl.NumberFormat("pt-BR", {
                style: "currency",
                currency: "BRL"
            }).format(amountDue)

            return <div className="text-right font-medium">{formatted}</div>
        },
    },
    {
        accessorKey: "amountPaid",
        header: () => <div className="text-right">Amount Paid</div>,
        cell: ({ row }) => {
            if (row.getValue("amountPaid")) {
                const amountPaid = parseFloat(row.getValue("amountPaid"))
                const formatted = new Intl.NumberFormat("pt-BR", {
                    style: "currency",
                    currency: "BRL"
                }).format(amountPaid)

                return <div className="text-right font-medium">{formatted}</div>
            }
        },
    },
    {
        accessorKey: "isPaid",
        header: "Paid?"
    }
]