import * as React from 'react';
import { NavMenu } from './NavMenu';

export class Layout extends React.Component<{}, {}> {
	public render() {
		return <div>
			<NavMenu />
			<div className="container">
				{this.props.children}
			</div>
		</div>;
	}
}
