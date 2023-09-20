import React, { Component } from 'react';

export class SumSup extends Component {
    static displayName = SumSup.name;

    constructor(props) {
        super(props);
        this.state = { northwinds: [], loading: true };
    }

    componentDidMount() {
        this.populateNorthwindData();
    }

    static renderNorthwindsTable(northwinds) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>ProductID</th>
                        <th>SupplierID</th>
                        <th>CompanyName</th>
                        <th>ProductName</th>
                        <th>SumPrice</th>
                    </tr>
                </thead>
                <tbody>
                    {northwinds.map(northwind =>
                        <tr key={northwind.productID}>
                            <td>{northwind.productID}</td>
                            <td>{northwind.supplierID}</td>
                            <td>{northwind.companyName}</td>
                            <td>{northwind.productName}</td>
                            <td>{northwind.sumPrice}</td>
                        </tr>
                    )}
                </tbody>
            </table>

        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : SumSup.renderNorthwindsTable(this.state.northwinds);

        return (
            <div>
                <h1 id="tabelLabel" >Northwind</h1>
                <p>This component demonstrates fetching data from the server.</p>
                {contents}
            </div>
        );
    }

    async populateNorthwindData() {
        const response = await fetch('http://localhost:5064/northwind/getSumSupplier',
            {
                method: 'POST',
            });
        const result = await response.json();
        this.setState({ northwinds: result, loading: false });
    }

}
