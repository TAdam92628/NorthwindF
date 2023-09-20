import React, { Component } from 'react';

export class FetchProducts extends Component {
    static displayName = FetchProducts.name;


  constructor(props) {
    super(props);
      this.state = { northwinds: [], loading: true };
      this.fetchFilteredProducts = this.fetchFilteredProducts.bind(this);
  }

  componentDidMount() {
    this.populateNorthwindData();
  }

  static renderNorthwindsTable(northwinds) {
      return (
             <table className='table table-striped' aria-labelledby="tabelLabel">
                  <thead>
                      <tr>
                          <th>Id</th>
                          <th>Name</th>
                          <th>QuantityPerUnit</th>
                          <th>UnitPrice</th>
                          <th>UnitsInStock</th>
                          <th>UnitsOnOrder</th>
                          <th>ReorderLevel</th>
                          <th>Discontinued</th>
                      </tr>
                  </thead>
                  <tbody>
                      {northwinds.map(northwind =>
                          <tr key={northwind.productID}>
                              <td>{northwind.productID}</td>
                              <td>{northwind.productName}</td>
                              <td>{northwind.quantityPerUnit}</td>
                              <td>{northwind.unitPrice}</td>
                              <td>{northwind.unitsInStock}</td>
                              <td>{northwind.unitsOnOrder}</td>
                              <td>{northwind.reorderLevel}</td>
                              <td>{northwind.discontinued}</td>
                          </tr>
                      )}
                  </tbody>
              </table>
      
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
        : FetchProducts.renderNorthwindsTable(this.state.northwinds);

    return (
      <div>
        <h1 id="tabelLabel" >Northwind</h1>
            <p>This component demonstrates fetching data from the server.</p>
            <input type='text' placeholder='Search Name...' className='search' onChange={this.fetchFilteredProducts} />

        {contents}
      </div>
    );
  }

    async populateNorthwindData() {
        
        const response = await fetch('http://localhost:5064/northwind');

        const data = await response.json();
        this.setState({ northwinds: data, loading: false });
    }

     async fetchFilteredProducts(e) {
         console.log(e.target.value);

         if (e.target.value != "") {
             const data = new FormData();
             data.append("name", e.target.value);

             const response = await fetch('http://localhost:5064/northwind/getFilteredProducts',
                 {
                     method: 'POST',
                     //headers: { 'Content-type': 'application/json' },
                     body: data
                 });
             const result = await response.json();
             this.setState({ northwinds: result, loading: false });
         }
         else {
             const response = await fetch('http://localhost:5064/northwind');

             const data = await response.json();
             this.setState({ northwinds: data, loading: false });
         }
        
    }

}
