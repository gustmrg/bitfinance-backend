import { Bill, columns } from "./columns"
import { DataTable } from "./data-table"

const http = require('http')

async function getData(): Promise<Bill[]> {
    // Fetch data from your API here.
    return [
        {
            id: 1,
            name: "Water Bill",
            amountDue: 80,
            isPaid: false
        },
        {
            id: 2,
            name: "Internet Bill",
            amountDue: 150,
            amountPaid: 150,
            isPaid: true
        },
        {
            id: 3,
            name: "Health Insurance",
            amountDue: 450,
            isPaid: false
        }
    ]
}

export default async function BillsPage() {
    const data = await getData()

    return (
        <div className="container mx-auto py-10">
            <DataTable columns={columns} data={data} />
        </div>
    )
}