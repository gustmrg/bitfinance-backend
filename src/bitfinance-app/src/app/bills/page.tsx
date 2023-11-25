import { Bill, columns } from "./columns"
import { DataTable } from "./data-table"

async function getData(): Promise<Bill[]> {
    // Fetch data from your API here.
    try {
        const res = await fetch('https://127.0.0.1:7267/bills', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        });
        const data = await res.json();
        return data;
    } catch (err) {
        console.log(err);
        return [];
    }
}

export default async function BillsPage() {
    const data = await getData()
    console.log(data)

    return (
        <div className="container mx-auto py-10">
            <DataTable columns={columns} data={data} />
        </div>
    )
}