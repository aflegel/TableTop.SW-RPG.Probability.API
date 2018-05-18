import * as React from "react";
import { RouteComponentProps } from "react-router-dom";

export default class About extends React.Component<RouteComponentProps<{}>, {}> {
	public render() {
		return <div className="row row-fill">
			<div className="col s12">
				<ul className="collection with-header">
					<li className="collection-header">
						<h2>About</h2>
					</li>
					<li className="collection-item">
					</li>
					<li className="collection-item">
					</li>
					<li className="collection-item">
					</li>
				</ul>
			</div>
		</div>;
	}
}
